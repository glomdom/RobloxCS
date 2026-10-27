--!strict

local ScopeAndState = {}
ScopeAndState.__index = ScopeAndState
export type ScopeAndStateFields = {
  ID: number,
}
type ScopeAndState = typeof(setmetatable({} :: ScopeAndStateFields, ScopeAndState))
ScopeAndState.GlobalCounter = 0
function ScopeAndState.new(): ScopeAndState
  local self = setmetatable({} :: ScopeAndStateFields, ScopeAndState)
  self.ID = 0
  return self
end
function ScopeAndState.Main(self: ScopeAndState): ()
  local instanceA: ScopeAndState = ScopeAndState.new()
  local instanceB: ScopeAndState = ScopeAndState.new()
  instanceA:SetID(10)
  instanceB:SetID(20)
  instanceA:IncrementGlobal()
  instanceB:IncrementGlobal()
  instanceA:PrintStatus()
  instanceB:PrintStatus()
  local ID: number = 999
  print(`local shadow id: {ID}`)
end
function ScopeAndState.SetID(self: ScopeAndState, val: number): ()
  self.ID = val
end
function ScopeAndState.IncrementGlobal(self: ScopeAndState): ()
  ScopeAndState.GlobalCounter += 1
end
function ScopeAndState.PrintStatus(self: ScopeAndState): ()
  print(`id: {self.ID}`)
  print(`global: {ScopeAndState.GlobalCounter}`)
end
local __entrypointCls = ScopeAndState.new()
__entrypointCls:Main()