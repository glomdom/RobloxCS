type _InstanceScopeAndState = {
  ID: number,
  constructor: (self: _InstanceScopeAndState) -> (),
}
type _TypeScopeAndState = {
  GlobalCounter: number,
  new: () -> _InstanceScopeAndState,
}
local ScopeAndState = {}
do
  ScopeAndState = setmetatable({}, {
    __tostring = function(): string
      return "ScopeAndState"
    end, 
  })
  ScopeAndState.__index = ScopeAndState
  ScopeAndState.GlobalCounter = 0
  function ScopeAndState.new(): _InstanceScopeAndState
    local self = setmetatable({}, ScopeAndState)
    self:constructor()
    return self
  end
  function ScopeAndState:constructor(): ()
    self.ID = 0
  end
  function ScopeAndState:Main(): ()
    local instanceA: _InstanceScopeAndState = ScopeAndState.new()
    local instanceB: _InstanceScopeAndState = ScopeAndState.new()
    instanceA:SetID(10)
    instanceB:SetID(20)
    instanceA:IncrementGlobal()
    instanceB:IncrementGlobal()
    instanceA:PrintStatus()
    instanceB:PrintStatus()
    local ID: number = 999
    print(`local shadow id: {ID}`)
  end
  function ScopeAndState:SetID(val: number): ()
    self.ID = val
  end
  function ScopeAndState:IncrementGlobal(): ()
    ScopeAndState.GlobalCounter += 1
  end
  function ScopeAndState:PrintStatus(): ()
    print(`id: {self.ID}`)
    print(`global: {ScopeAndState.GlobalCounter}`)
  end
end
local __entrypointCls = ScopeAndState.new()
__entrypointCls:Main()
