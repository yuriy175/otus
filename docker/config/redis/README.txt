1. Run redis cli
redis-cli

2. Register lua functions
FUNCTION "load" "replace" "#!lua name=posts \n redis.register_function('getposts', \n function (keys, args) \n local posts=redis.call('lrange', keys[1], args[1], args[2]) \n return posts \n end) \n redis.register_function('warmupposts',  \n function (keys, args)  \n local count=redis.call('rpush', keys[1], unpack(args)) \n return count \n end) \n redis.register_function('addpost', \n function (keys, args)  \n local count=redis.call('lpush', keys[1], args[1])  \n count=redis.call('ltrim', keys[1], 0, args[2]) \n return count \n end)"

