package dto

// UserPostsDto represents dto for a user posts
type UserPostsDto struct {
	Authors []*UserDto `json:"authors"`
	Posts   []*PostDto `json:"posts"`
}
