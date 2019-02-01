# !bin/sh

# C#
/usr/local/bin/protoc -I ./Proto --csharp_out=./TodoAPi-Dapper/Models/Proto --grpc_out=./TodoAPi-Dapper/Models/Proto  --plugin=protoc-gen-grpc=/usr/local/bin/grpc_csharp_plugin ./Proto/todo.proto