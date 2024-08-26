export interface AuthInfo {
  IsAuthenticated: boolean;
  NeedLogin: boolean;
  token: string | null;
  user: string | null;
  environment: string | null;
}

export function userNotAuthenticated(environment: string = ''): AuthInfo {
  return {
    IsAuthenticated: false,
    NeedLogin: true,
    token: null,
    user: null,
    environment: environment
  };
}

export function userAuthenticated(needLogin: boolean, token: string, user: string, environment: string): AuthInfo {
  return {
    IsAuthenticated: true,
    NeedLogin: needLogin,
    token: token,
    user: user,
    environment: environment
  };
}
