package dialogservice

import (
	"os"

	_ "github.com/google/uuid"
	"socialnerworkapp.com/bff/internal/service"
)

type dialogServiceImp struct {
	grpcDialogUrl  string
	grpcProfileUrl string
}

func NewDialogService() service.DialogService {
	grpcDialogUrl, _ := os.LookupEnv("GRPC_DIALOGS")
	grpcProfileUrl, _ := os.LookupEnv("GRPC_PROFILE")
	return &dialogServiceImp{grpcDialogUrl: grpcDialogUrl, grpcProfileUrl: grpcProfileUrl}
}
