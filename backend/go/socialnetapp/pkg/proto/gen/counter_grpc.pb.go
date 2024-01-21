// Code generated by protoc-gen-go-grpc. DO NOT EDIT.
// versions:
// - protoc-gen-go-grpc v1.2.0
// - protoc             v4.25.1
// source: counter.proto

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

// CounterClient is the client API for Counter service.
//
// For semantics around ctx use and closing/ending streaming RPCs, please refer to https://pkg.go.dev/google.golang.org/grpc/?tab=doc#ClientConn.NewStream.
type CounterClient interface {
	GetUnreadCount(ctx context.Context, in *GetUnreadCountRequest, opts ...grpc.CallOption) (*GetUnreadCountReply, error)
}

type counterClient struct {
	cc grpc.ClientConnInterface
}

func NewCounterClient(cc grpc.ClientConnInterface) CounterClient {
	return &counterClient{cc}
}

func (c *counterClient) GetUnreadCount(ctx context.Context, in *GetUnreadCountRequest, opts ...grpc.CallOption) (*GetUnreadCountReply, error) {
	out := new(GetUnreadCountReply)
	err := c.cc.Invoke(ctx, "/Counter/GetUnreadCount", in, out, opts...)
	if err != nil {
		return nil, err
	}
	return out, nil
}

// CounterServer is the server API for Counter service.
// All implementations must embed UnimplementedCounterServer
// for forward compatibility
type CounterServer interface {
	GetUnreadCount(context.Context, *GetUnreadCountRequest) (*GetUnreadCountReply, error)
	mustEmbedUnimplementedCounterServer()
}

// UnimplementedCounterServer must be embedded to have forward compatible implementations.
type UnimplementedCounterServer struct {
}

func (UnimplementedCounterServer) GetUnreadCount(context.Context, *GetUnreadCountRequest) (*GetUnreadCountReply, error) {
	return nil, status.Errorf(codes.Unimplemented, "method GetUnreadCount not implemented")
}
func (UnimplementedCounterServer) mustEmbedUnimplementedCounterServer() {}

// UnsafeCounterServer may be embedded to opt out of forward compatibility for this service.
// Use of this interface is not recommended, as added methods to CounterServer will
// result in compilation errors.
type UnsafeCounterServer interface {
	mustEmbedUnimplementedCounterServer()
}

func RegisterCounterServer(s grpc.ServiceRegistrar, srv CounterServer) {
	s.RegisterService(&Counter_ServiceDesc, srv)
}

func _Counter_GetUnreadCount_Handler(srv interface{}, ctx context.Context, dec func(interface{}) error, interceptor grpc.UnaryServerInterceptor) (interface{}, error) {
	in := new(GetUnreadCountRequest)
	if err := dec(in); err != nil {
		return nil, err
	}
	if interceptor == nil {
		return srv.(CounterServer).GetUnreadCount(ctx, in)
	}
	info := &grpc.UnaryServerInfo{
		Server:     srv,
		FullMethod: "/Counter/GetUnreadCount",
	}
	handler := func(ctx context.Context, req interface{}) (interface{}, error) {
		return srv.(CounterServer).GetUnreadCount(ctx, req.(*GetUnreadCountRequest))
	}
	return interceptor(ctx, in, info, handler)
}

// Counter_ServiceDesc is the grpc.ServiceDesc for Counter service.
// It's only intended for direct use with grpc.RegisterService,
// and not to be introspected or modified (even as a copy)
var Counter_ServiceDesc = grpc.ServiceDesc{
	ServiceName: "Counter",
	HandlerType: (*CounterServer)(nil),
	Methods: []grpc.MethodDesc{
		{
			MethodName: "GetUnreadCount",
			Handler:    _Counter_GetUnreadCount_Handler,
		},
	},
	Streams:  []grpc.StreamDesc{},
	Metadata: "counter.proto",
}
