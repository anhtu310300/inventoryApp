export interface LoginResponse {
  accessToken: string;
  tokenType: string;
  userId: number;
  employeeCode: string;
  fullName: string;
  email: string;
  role: string;
}