  export type ResponseStatus =
  | "Success"
  | "Error"
  | "NotFound"
  | "CriticalError";

export interface LoginRequest{
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
}

export interface ResponsePattern<T = unknown> {
  message?: string;
  status: ResponseStatus;
  content: T;
}