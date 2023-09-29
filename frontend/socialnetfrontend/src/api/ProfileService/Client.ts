//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.20.0.0 (NJsonSchema v10.9.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

import axios, { AxiosError, AxiosInstance, AxiosRequestConfig, AxiosResponse, CancelToken } from 'axios';

export class Client {
    private instance: AxiosInstance;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, instance?: AxiosInstance) {

        this.instance = instance ? instance : axios.create();

        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "/";

    }

    /**
     * Login user
     * @param properties Login properties
     * @return OK
     */
    login(properties: LoginDto, cancelToken?: CancelToken | undefined): Promise<string> {
        let url_ = this.baseUrl + "/login";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(properties);

        let options_: AxiosRequestConfig = {
            data: content_,
            method: "POST",
            url: url_,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            cancelToken
        };

        return this.instance.request(options_).catch((_error: any) => {
            if (isAxiosError(_error) && _error.response) {
                return _error.response;
            } else {
                throw _error;
            }
        }).then((_response: AxiosResponse) => {
            return this.processLogin(_response);
        });
    }

    protected processLogin(response: AxiosResponse): Promise<string> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
                result200 = resultData200 !== undefined ? resultData200 : <any>null;
    
            return Promise.resolve<string>(result200);

        } else if (status === 400) {
            const _responseText = response.data;
            return throwException("Bad Request", status, _responseText, _headers);

        } else if (status === 500) {
            const _responseText = response.data;
            return throwException("Internal Server Error", status, _responseText, _headers);

        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<string>(null as any);
    }

    /**
     * Get user by id
     * @param id User id
     * @return OK
     */
    get(id: string, cancelToken?: CancelToken | undefined): Promise<User> {
        let url_ = this.baseUrl + "/user/get/{id}";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");

        let options_: AxiosRequestConfig = {
            method: "GET",
            url: url_,
            headers: {
                "Accept": "application/json"
            },
            cancelToken
        };

        return this.instance.request(options_).catch((_error: any) => {
            if (isAxiosError(_error) && _error.response) {
                return _error.response;
            } else {
                throw _error;
            }
        }).then((_response: AxiosResponse) => {
            return this.processGet(_response);
        });
    }

    protected processGet(response: AxiosResponse): Promise<User> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
            result200 = User.fromJS(resultData200);
            return Promise.resolve<User>(result200);

        } else if (status === 400) {
            const _responseText = response.data;
            return throwException("Bad Request", status, _responseText, _headers);

        } else if (status === 404) {
            const _responseText = response.data;
            return throwException("Not Found", status, _responseText, _headers);

        } else if (status === 500) {
            const _responseText = response.data;
            return throwException("Internal Server Error", status, _responseText, _headers);

        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<User>(null as any);
    }

    /**
     * Register new user
     * @param properties User properties
     * @return OK
     */
    register(properties: NewUserDto, cancelToken?: CancelToken | undefined): Promise<string> {
        let url_ = this.baseUrl + "/user/register";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(properties);

        let options_: AxiosRequestConfig = {
            data: content_,
            method: "POST",
            url: url_,
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            cancelToken
        };

        return this.instance.request(options_).catch((_error: any) => {
            if (isAxiosError(_error) && _error.response) {
                return _error.response;
            } else {
                throw _error;
            }
        }).then((_response: AxiosResponse) => {
            return this.processRegister(_response);
        });
    }

    protected processRegister(response: AxiosResponse): Promise<string> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
                result200 = resultData200 !== undefined ? resultData200 : <any>null;
    
            return Promise.resolve<string>(result200);

        } else if (status === 400) {
            const _responseText = response.data;
            return throwException("Bad Request", status, _responseText, _headers);

        } else if (status === 404) {
            const _responseText = response.data;
            return throwException("Not Found", status, _responseText, _headers);

        } else if (status === 500) {
            const _responseText = response.data;
            return throwException("Internal Server Error", status, _responseText, _headers);

        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<string>(null as any);
    }

    /**
     * Find user
     * @param first_name (optional) 
     * @param last_name (optional) 
     * @return OK
     */
    search(first_name: string | null | undefined, last_name: string | null | undefined, cancelToken?: CancelToken | undefined): Promise<User[]> {
        let url_ = this.baseUrl + "/user/search?";
        if (first_name !== undefined && first_name !== null)
            url_ += "first_name=" + encodeURIComponent("" + first_name) + "&";
        if (last_name !== undefined && last_name !== null)
            url_ += "last_name=" + encodeURIComponent("" + last_name) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_: AxiosRequestConfig = {
            method: "GET",
            url: url_,
            headers: {
                "Accept": "application/json"
            },
            cancelToken
        };

        return this.instance.request(options_).catch((_error: any) => {
            if (isAxiosError(_error) && _error.response) {
                return _error.response;
            } else {
                throw _error;
            }
        }).then((_response: AxiosResponse) => {
            return this.processSearch(_response);
        });
    }

    protected processSearch(response: AxiosResponse): Promise<User[]> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(User.fromJS(item));
            }
            else {
                result200 = <any>null;
            }
            return Promise.resolve<User[]>(result200);

        } else if (status === 400) {
            const _responseText = response.data;
            return throwException("Bad Request", status, _responseText, _headers);

        } else if (status === 404) {
            const _responseText = response.data;
            return throwException("Not Found", status, _responseText, _headers);

        } else if (status === 500) {
            const _responseText = response.data;
            return throwException("Internal Server Error", status, _responseText, _headers);

        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<User[]>(null as any);
    }

    /**
     * Get all users
     * @return OK
     */
    users( cancelToken?: CancelToken | undefined): Promise<User[]> {
        let url_ = this.baseUrl + "/users";
        url_ = url_.replace(/[?&]$/, "");

        let options_: AxiosRequestConfig = {
            method: "GET",
            url: url_,
            headers: {
                "Accept": "application/json"
            },
            cancelToken
        };

        return this.instance.request(options_).catch((_error: any) => {
            if (isAxiosError(_error) && _error.response) {
                return _error.response;
            } else {
                throw _error;
            }
        }).then((_response: AxiosResponse) => {
            return this.processUsers(_response);
        });
    }

    protected processUsers(response: AxiosResponse): Promise<User[]> {
        const status = response.status;
        let _headers: any = {};
        if (response.headers && typeof response.headers === "object") {
            for (let k in response.headers) {
                if (response.headers.hasOwnProperty(k)) {
                    _headers[k] = response.headers[k];
                }
            }
        }
        if (status === 200) {
            const _responseText = response.data;
            let result200: any = null;
            let resultData200  = _responseText;
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(User.fromJS(item));
            }
            else {
                result200 = <any>null;
            }
            return Promise.resolve<User[]>(result200);

        } else if (status === 500) {
            const _responseText = response.data;
            return throwException("Internal Server Error", status, _responseText, _headers);

        } else if (status !== 200 && status !== 204) {
            const _responseText = response.data;
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<User[]>(null as any);
    }
}

export class LoginDto implements ILoginDto {
    id?: number | undefined;
    password?: string | undefined;

    constructor(data?: ILoginDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
            this.password = _data["password"];
        }
    }

    static fromJS(data: any): LoginDto {
        data = typeof data === 'object' ? data : {};
        let result = new LoginDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["password"] = this.password;
        return data;
    }
}

export interface ILoginDto {
    id?: number | undefined;
    password?: string | undefined;
}

export class NewUserDto implements INewUserDto {
    age?: number | undefined;
    city?: string | undefined;
    info?: string | undefined;
    name?: string | undefined;
    password?: string | undefined;
    sex?: string | undefined;
    surname?: string | undefined;

    constructor(data?: INewUserDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.age = _data["age"];
            this.city = _data["city"];
            this.info = _data["info"];
            this.name = _data["name"];
            this.password = _data["password"];
            this.sex = _data["sex"];
            this.surname = _data["surname"];
        }
    }

    static fromJS(data: any): NewUserDto {
        data = typeof data === 'object' ? data : {};
        let result = new NewUserDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["age"] = this.age;
        data["city"] = this.city;
        data["info"] = this.info;
        data["name"] = this.name;
        data["password"] = this.password;
        data["sex"] = this.sex;
        data["surname"] = this.surname;
        return data;
    }
}

export interface INewUserDto {
    age?: number | undefined;
    city?: string | undefined;
    info?: string | undefined;
    name?: string | undefined;
    password?: string | undefined;
    sex?: string | undefined;
    surname?: string | undefined;
}

export class User implements IUser {
    age?: number | undefined;
    city?: string | undefined;
    id?: number | undefined;
    info?: string | undefined;
    name?: string | undefined;
    sex?: number | undefined;
    surname?: string | undefined;

    constructor(data?: IUser) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.age = _data["age"];
            this.city = _data["city"];
            this.id = _data["id"];
            this.info = _data["info"];
            this.name = _data["name"];
            this.sex = _data["sex"];
            this.surname = _data["surname"];
        }
    }

    static fromJS(data: any): User {
        data = typeof data === 'object' ? data : {};
        let result = new User();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["age"] = this.age;
        data["city"] = this.city;
        data["id"] = this.id;
        data["info"] = this.info;
        data["name"] = this.name;
        data["sex"] = this.sex;
        data["surname"] = this.surname;
        return data;
    }
}

export interface IUser {
    age?: number | undefined;
    city?: string | undefined;
    id?: number | undefined;
    info?: string | undefined;
    name?: string | undefined;
    sex?: number | undefined;
    surname?: string | undefined;
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new ApiException(message, status, response, headers, null);
}

function isAxiosError(obj: any | undefined): obj is AxiosError {
    return obj && obj.isAxiosError === true;
}