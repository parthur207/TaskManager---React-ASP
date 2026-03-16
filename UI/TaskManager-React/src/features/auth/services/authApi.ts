import { api } from "../../../infra/http";
import type { LoginRequest, ResponsePattern } from "../types";

export async function login(data: LoginRequest) {
  const response = await api.post<ResponsePattern<string>>(
    "/login",
    data
  );

  return response.data;
}
