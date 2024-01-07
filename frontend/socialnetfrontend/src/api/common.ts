import type {AxiosRequestConfig} from 'axios'
import axios from 'axios'

const baseCsURL = '/api'
const baseGoURL = '/api'

export const axiosCsInstance = axios.create({
    baseURL:baseCsURL,
    responseType: 'json',
    transformResponse: r => r
})

export const axiosGoInstance = axios.create({
    baseURL:baseGoURL,
    responseType: 'json',
    transformResponse: r => r
})

export const addBearerToken = (token: string) => {
    axiosCsInstance.interceptors.request.use(config => {
        config.headers.authorization = `Bearer ${token}`
        return config
    })

    axiosGoInstance.interceptors.request.use(config => {
        config.headers.authorization = `Bearer ${token}`
        return config
    })
}