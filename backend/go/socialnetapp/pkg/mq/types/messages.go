package types

// MessageHeader represents header for a MQ message
type MessageHeader struct {
	MessageType uint `json:"messageType"`
}

type UnreadCountMessage struct {
	MessageHeader
	IsIncrement      bool  `json:"isIncrement"`
	UserId           uint  `json:"userId"`
	UnreadMessageIds []int `json:"unreadMessageIds"`
}
