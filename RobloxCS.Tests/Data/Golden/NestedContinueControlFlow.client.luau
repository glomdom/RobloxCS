type _InstanceNestedContinueControlFlow = {
  constructor: (self: _InstanceNestedContinueControlFlow) -> (),
}
type _TypeNestedContinueControlFlow = {
  new: () -> _InstanceNestedContinueControlFlow,
}
local NestedContinueControlFlow = {}
do
  NestedContinueControlFlow = setmetatable({}, {
    __tostring = function(): string
      return "NestedContinueControlFlow"
    end, 
  })
  NestedContinueControlFlow.__index = NestedContinueControlFlow
  function NestedContinueControlFlow.new(): _InstanceNestedContinueControlFlow
    local self = setmetatable({}, NestedContinueControlFlow)
    self:constructor()
    return self
  end
  function NestedContinueControlFlow:constructor(): ()
  end
  function NestedContinueControlFlow:Main(): ()
    do
      local x: number = 0
      while true do
        if not (x < 3) then
          break
        end
        do
          local y: number = 0
          while true do
            if not (y < 3) then
              break
            end
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
    repeat
      print(count)
      count -= 1
      if count == 1 then
        print("Hit 1")
      end
    until not (count > 0)
  end
end
local __entrypointCls = NestedContinueControlFlow.new()
__entrypointCls:Main()
