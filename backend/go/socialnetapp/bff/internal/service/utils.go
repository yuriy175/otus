package service

import (
	"time"

	"google.golang.org/protobuf/types/known/timestamppb"
	"socialnerworkapp.com/bff/internal/handler/dto"
	"socialnerworkapp.com/pkg/proto/gen"
)

func ConvertToUserDto(user *gen.UserReply) *dto.UserDto {
	userDto := &dto.UserDto{
		ID:      uint(user.Id),
		Name:    user.Name,
		Surname: user.Surname,
		//Age:     user.Age.Value,
		//Sex:     user.Sex,
	}
	if user.City != nil {
		userDto.City = &user.City.Value
	}
	if user.Info != nil {
		userDto.Info = &user.Info.Value
	}
	return userDto
}

func ConvertToTime(x *timestamppb.Timestamp) *time.Time {
	t := time.Unix(int64(x.GetSeconds()), int64(x.GetNanos())).UTC()
	return &t
}
