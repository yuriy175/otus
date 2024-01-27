package mq

const postChannelName string = "/post/feed/posted"
const dialogWebsockQueueName string = "dialogwebsockqueue"
const counterQueueName string = "countersqueue"
const counterDialogQueueName string = "counterdialogqueue"

const (
	NewDialogMessage                     = 1
	UpdateUnreadDialogMessages           = 2
	UpdateUnreadDialogMessagesCompensate = 3
)
