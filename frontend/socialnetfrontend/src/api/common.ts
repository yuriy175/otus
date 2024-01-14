import type {AxiosRequestConfig} from 'axios'
import axios from 'axios'

const baseURL = '/api'

export const axiosInstance = axios.create({
    baseURL:baseURL,
    responseType: 'json',
    transformResponse: r => r
})

export const addBearerToken = (token: string) => {
    axiosInstance.interceptors.request.use(config => {
        config.headers.authorization = `Bearer ${token}`
        return config
    })
}