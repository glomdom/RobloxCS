--!strict

local StaticTest = {}
StaticTest.__index = StaticTest
type StaticTestFields = {
  _x: number,
}
export type StaticTest = typeof(setmetatable({} :: StaticTestFields, StaticTest))
function StaticTest.new(): StaticTest
  local self = setmetatable({} :: StaticTestFields, StaticTest)
  self._x = 2
  return self
end
function StaticTest.Main(self: StaticTest): ()
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
local __entrypointCls = StaticTest.new()
__entrypointCls:Main()