type _InstanceStaticTest = {
  constructor: (self: _InstanceStaticTest) -> (),
}
type _TypeStaticTest = {
  new: () -> _InstanceStaticTest,
}
local StaticTest = {}
do
  StaticTest = setmetatable({}, {
    __tostring = function(): string
      return "StaticTest"
    end, 
  })
  StaticTest.__index = StaticTest
  function StaticTest.new(): _InstanceStaticTest
    local self = setmetatable({}, StaticTest)
    self:constructor()
    return self
  end
  function StaticTest:constructor(): ()
  end
  function StaticTest:Main(): ()
    local greeting: string = "slice"
    local a: number, b: number = 1, 2
    local c: number
    c = 3
    print(greeting)
    print(a)
    print(b)
    print(c)
    StaticTest.Echo(42)
    StaticTest.Echo(c)
  end
  function StaticTest.Echo(value: number): ()
    print(value)
  end
end
local __entrypointCls = StaticTest.new()
__entrypointCls:Main()
