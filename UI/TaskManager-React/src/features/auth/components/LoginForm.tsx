import { useLogin } from "../hooks/useLogin";

export default function LoginForm() {
  const { form, handleChange, handleSubmit, loading } = useLogin();

  return (
    <form onSubmit={handleSubmit}>
      <h1>Bem-vindo de volta!</h1>

      <input
        name="email"
        placeholder="Email"
        value={form.email}
        onChange={handleChange}
      />

      <input
        name="password"
        type="password"
        placeholder="Senha"
        value={form.password}
        onChange={handleChange}
      />

      <button disabled={loading}>
        {loading ? "Entrando..." : "Entrar"}
      </button>
    </form>
  );
}
