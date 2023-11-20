namespace Posts.API.Daos
{
    public readonly record  struct NewPostDao
    {
        public int UserId { get; init; }
        public string Message { get; init; }
    }
}
