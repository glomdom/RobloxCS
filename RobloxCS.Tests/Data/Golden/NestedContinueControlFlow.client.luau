--!strict

local NestedContinueControlFlow = {}
NestedContinueControlFlow.__index = NestedContinueControlFlow
type NestedContinueControlFlow = typeof(setmetatable({}, NestedContinueControlFlow))
function NestedContinueControlFlow.new(): NestedContinueControlFlow
  local self = setmetatable({}, NestedContinueControlFlow)
  return self
end
function NestedContinueControlFlow.Main(self: NestedContinueControlFlow): ()
  do
    local x: number = 0
    while x < 3 do
      do
        local y: number = 0
        while y < 3 do
          if x == 1 and y == 1 then
            print("Skipping Center")
            y += 1
            continue
          end
          if x == 2 and y == 2 then
            print("Breaking Inner")
            break
          end
          print(x)
          print(y)
          y += 1
        end
      end
      x += 1
    end
  end
  local count: number = 3
  while count > 0 do
    print(count)
    count -= 1
    if count == 1 then
      print("Hit 1")
    end
  end
end
local __entrypointCls = NestedContinueControlFlow.new()
__entrypointCls:Main()