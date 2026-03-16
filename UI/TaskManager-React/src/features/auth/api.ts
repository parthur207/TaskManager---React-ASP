import type { ResponsePattern , LoginRequest,  ResponseStatus} from "./types";



export async function Login(data: LoginRequest) : Promise<ResponsePattern>{

    try
    {
        const Response= await api.post<ResponsePattern>("auth/login", data);
    }
    catch()
    {
        
    }
    return Response;
}

 

if(Response.Status==ResponseStatus.)

  }
 