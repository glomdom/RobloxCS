--!strict

local Hello = {}
Hello.__index = Hello
type Hello = typeof(setmetatable({}, Hello))
function Hello.new(): Hello
  local self = setmetatable({}, Hello)
  return self
end
function Hello.Main(self: Hello): ()
  print("hello from RobloxCS")
end
local __entrypointCls = Hello.new()
__entrypointCls:Main()
