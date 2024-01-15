import {axiosInstance} from '../common'

import {CountersClient} from '../Client'


const getCountersClient = (): CountersClient => new CountersClient('', axiosInstance) 

export const getUnreadMessageCount = async () => {
    const client = getCountersClient()
    return client.count()
}