using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TaskManager.Core.Events;
using TaskManager.Core.Ports.Emails;

namespace TaskManager.Adapters.ExternalServices.Messaging
{
    public class TwoFactorEmailConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqConnectionProvider _connectionProvider;
        private readonly RabbitMqSettings _settings;
        private IChannel _channel;

        public TwoFactorEmailConsumer(
            IServiceProvider serviceProvider,
            RabbitMqConnectionProvider connectionProvider,
            IOptions<RabbitMqSettings> options)
        {
            _serviceProvider = serviceProvider;
            _connectionProvider = connectionProvider;
            _settings = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _channel = await _connectionProvider.Connection.CreateChannelAsync();

                await _channel.ExchangeDeclareAsync(
                    exchange: _settings.ExchangeName,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false);

                await _channel.QueueDeclareAsync(
                    queue: "taskmanager.2fa.email",
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                await _channel.QueueBindAsync(
                    queue: "taskmanager.2fa.email",
                    exchange: _settings.ExchangeName,
                    routingKey: "2fa.*");

                await _channel.BasicQosAsync(0, 1, false);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var twoFactorEvent = JsonSerializer.Deserialize<TwoFactorEmailEvent>(message);

                        if (twoFactorEvent != null)
                        {
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSenderPort>();

                                await emailSender.Send2FAEmailAsync(
                                    twoFactorEvent.Email,
                                    twoFactorEvent.Code);
                            }
                        }

                        await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                    }
                };

                await _channel.BasicConsumeAsync(
                    queue: "taskmanager.2fa.email",
                    autoAck: false,
                    consumerTag: "twoFactorEmailConsumer",
                    consumer: consumer);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro inesperado: "+ex.Message);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel?.IsOpen ?? false)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }
            await base.StopAsync(cancellationToken);
        }
    }
}
