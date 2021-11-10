import axios from "axios";

const API_URL = "https://localhost:44389/api/";
const ACCOUNT_URL = "account";
const AUTH_TOKEN  = "authorization_token";

class AuthenticationService {
    public login(email: string, password: string) {
        return axios
            .post(
                API_URL + ACCOUNT_URL + "login",
                {
                    email,
                    password
                })
            .then(response => {
                if (response.data.access_token) {
                    console.log(response.data.access_token);
                    localStorage.setItem(AUTH_TOKEN, response.data.access_token);
                }

                return response.data;
            });
    }

    public logout() {
        localStorage.removeItem(AUTH_TOKEN);
    }
}

export default AuthenticationService