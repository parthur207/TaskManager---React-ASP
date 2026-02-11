import { create } from "zustand";

interface AuthState {
  token?: string;
  userId?: string;
  role?: string;

  isAuthenticated: boolean;

  setAuth: (data: {
    token: string;
    userId: string;
    role: string;
  }) => void;

  logout: () => void;
}

export const useAuthStore = create<AuthState>(set => ({
  token: undefined,
  userId: undefined,
  role: undefined,
  isAuthenticated: false,

  setAuth: ({ token, userId, role }) =>
    set({
      token,
      userId,
      role,
      isAuthenticated: true
    }),

  logout: () =>
    set({
      token: undefined,
      userId: undefined,
      role: undefined,
      isAuthenticated: false
    })
}));
