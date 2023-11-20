package dto

// UpdatePostDto represents dto for updating a post
type UpdatePostDto struct {
	PostId uint   `json:"id"`
	Text   string `json:"text"`
}
