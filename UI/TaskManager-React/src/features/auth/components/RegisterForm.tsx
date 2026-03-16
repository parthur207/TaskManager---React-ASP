import {useRegister} from "../hooks/useRegister";

export default function RegisterForm() 
{
  const { form, handleChange, handleSubmit, loading } = useRegister();


  return(
    <form onSubmit={handleSubmit}>
      <h1>Crie sua conta!</h1>

      <input
        name="name"
        placeholder="Nome"
        value={form.name}
        onChange={handleChange}
        type="text"
      />

      <input 
        name="email"
        placeholder="Email"
        value={form.email}
        onChange={handleChange}
        type="email"
      />

      <input
        name="password"
        type="password"
        placeholder="Senha"
        value={form.password}
        onChange={handleChange}
      />
      <input
      name=" "
      />
      <button disabled={loading}>
        {loading ? "Cadastrando..." : "Cadastrar"}
      </button> 
      </form>
  )
}