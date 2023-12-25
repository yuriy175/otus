// Code generated by protoc-gen-go-grpc. DO NOT EDIT.
// versions:
// - protoc-gen-go-grpc v1.2.0
// - protoc             v4.25.1
// source: dialog.proto

package gen

import (
	context "context"
	grpc "google.golang.org/grpc"
	codes "google.golang.org/grpc/codes"
	status "google.golang.org/grpc/status"
)

// This is a compile-time assertion to ensure that this generated file
// is compatible with the grpc package it is being compiled against.
// Requires gRPC-Go v1.32.0 or later.
const _ = grpc.SupportPackageIsVersion7

// DialogClient is the client API for Dialog service.
//
// For semantics around ctx use and closing/ending streaming RPCs, please refer to https://pkg.go.dev/google.golang.org/grpc/?tab=doc#ClientConn.NewStream.
type DialogClient interface {
	GetMessages(ctx context.Context, in *GetMessagesRequest, opts ...grpc.CallOption) (*GetMessagesReply, error)
	CreateMessage(ctx context.Context, in *CreateMessageRequest, opts ...grpc.CallOption) (*MessageReply, error)
}

type dialogClient struct {
	cc grpc.ClientConnInterface
}

func NewDialogClient(cc grpc.ClientConnInterface) DialogClient {
	return &dialogClient{cc}
}

func (c *dialogClient) GetMessages(ctx context.Context, in *GetMessagesRequest, opts ...grpc.CallOption) (*GetMessagesReply, error) {
	out := new(GetMessagesReply)
	err := c.cc.Invoke(ctx, "/Dialog/GetMessages", in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

func (c *dialogClient) CreateMessage(ctx context.Context, in *CreateMessageRequest, opts ...grpc.CallOption) (*MessageReply, error) {
	out := new(MessageReply)
	err := c.cc.Invoke(ctx, "/Dialog/CreateMessage", in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

// DialogServer is the server API for Dialog service.
// All implementations must embed UnimplementedDialogServer
// for forward compatibility
type DialogServer interface {
	GetMessages(context.Context, *GetMessagesRequest) (*GetMessagesReply, error)
	CreateMessage(context.Context, *CreateMessageRequest) (*MessageReply, error)
	mustEmbedUnimplementedDialogServer()
}

// UnimplementedDialogServer must be embedded to have forward compatible implementations.
type UnimplementedDialogServer struct {
}

func (UnimplementedDialogServer) GetMessages(context.Context, *GetMessagesRequest) (*GetMessagesReply, error) {
	return nil, status.Errorf(codes.Unimplemented, "method GetMessages not implemented")
}
func (UnimplementedDialogServer) CreateMessage(context.Context, *CreateMessageRequest) (*MessageReply, error) {
	return nil, status.Errorf(codes.Unimplemented, "method CreateMessage not implemented")
}
func (UnimplementedDialogServer) mustEmbedUnimplementedDialogServer() {}

// UnsafeDialogServer may be embedded to opt out of forward compatibility for this service.
// Use of this interface is not recommended, as added methods to DialogServer will
// result in compilation errors.
type UnsafeDialogServer interface {
	mustEmbedUnimplementedDialogServer()
}

func RegisterDialogServer(s grpc.ServiceRegistrar, srv DialogServer) {
	s.RegisterService(&Dialog_ServiceDesc, srv)
}

func _Dialog_GetMessages_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(GetMessagesRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(DialogServer).GetMessages(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: "/Dialog/GetMessages",
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(DialogServer).GetMessages(ctx, req.(*GetMessagesRequest))
	}
	return interceptor(ctx, in, info, handler)
}

func _Dialog_CreateMessage_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(CreateMessageRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(DialogServer).CreateMessage(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: "/Dialog/CreateMessage",
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(DialogServer).CreateMessage(ctx, req.(*CreateMessageRequest))
	}
	return interceptor(ctx, in, info, handler)
}

// Dialog_ServiceDesc is the grpc.ServiceDesc for Dialog service.
// It's only intended for direct use with grpc.RegisterService,
// and not to be introspected or modified (even as a copy)
var Dialog_ServiceDesc = grpc.ServiceDesc{
	ServiceName: "Dialog",
	HandlerType: (*DialogServer)(nil),
	Methods: []grpc.MethodDesc{
		{
			MethodName: "GetMessages",
			Handler:    _Dialog_GetMessages_Handler,
		},
		{
			MethodName: "CreateMessage",
			Handler:    _Dialog_CreateMessage_Handler,
		},
	},
	Streams:  []grpc.StreamDesc{},
	Metadata: "dialog.proto",
}