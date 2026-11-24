--!strict

-- Runtime type descriptors for reified generics. Every type argument is passed around as one of these.

export type AssignableTypes = { [string]: boolean }

export type TypeInfo = {
  key: string,
  name: string,
  valueType: boolean,
  default: any,
  assignable: AssignableTypes,
  -- returns the value (unboxed for value types) when it is of this type, else nil
  test: (value: any) -> any,
  new: (() -> any)?,
}

local Types = {}

local typeInfos: { [string]: TypeInfo } = {}

-- one descriptor per type key, shared by every module
function Types.typeInfo(key: string, build: () -> any): any
  local info = typeInfos[key]
  if info == nil then
    info = build() :: TypeInfo
    typeInfos[key] = info
  end

  return info
end

function Types.typeCast(info: any, value: any, allowNil: boolean): any
  local target = info :: TypeInfo
  if value == nil then
    if target.valueType and not allowNil then
      error("Invalid cast to " .. target.name)
    end

    return nil
  end

  local result = target.test(value)
  if result == nil then
    error("Invalid cast to " .. target.name)
  end

  return result
end

return Types
