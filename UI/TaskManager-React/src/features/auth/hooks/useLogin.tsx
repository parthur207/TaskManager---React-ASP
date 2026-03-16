import { useState } from "react";
import type { LoginRequest } from "../types";
import { login } from "../services/authApi";

export function useLogin() {
  const [form, setForm] = useState<LoginRequest>({
    email: "",
    password: ""
  });

  const [loading, setLoading] = useState(false);

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    setForm({ ...form, [e.target.name]: e.target.value });
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);

    await login(form);

    setLoading(false);
  }

  return { form, handleChange, handleSubmit, loading };
}
