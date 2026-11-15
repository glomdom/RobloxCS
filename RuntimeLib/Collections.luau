--!strict

export type Kind = "array" | "list" | "readonly"
export type CastKind = Kind | "interface" | "mutableInterface"

export type AssignableTypes = { [string]: boolean }

export type Descriptor = {
  __arrayName: string,
  __listName: string,
  __element: string,
  __assignable: AssignableTypes,
}

export type Metadata = Descriptor & {
  __kind: Kind,
  __name: string,
}

export type Sequence<T> = {
  items: { T },
  count: number,
  capacity: number,
  version: number,
  array: boolean,
  readonly: boolean,
  read accept: (value: unknown) -> boolean,
  -- C# reference-type defaults may be nil even when T is not nullable.
  read default: T?,
}

local Collections = {}

type RuntimeTable = { [string]: unknown }

local function runtimeMetadata(value: unknown): RuntimeTable?
  if typeof(value) ~= "table" then
    return nil
  end

  local class: unknown = getmetatable(value)
  if typeof(class) ~= "table" then
    return nil
  end

  local metadata: unknown = getmetatable(class)
  return if typeof(metadata) == "table" then metadata :: RuntimeTable else nil
end

local function metadataOf<T>(sequence: Sequence<T>): Metadata
  local metadata = runtimeMetadata(sequence)
  if metadata == nil then
    error("Collection metadata is missing.")
  end

  return metadata :: Metadata
end

function Collections.cast<T>(value: unknown, element: string, kind: CastKind, covariant: boolean): Sequence<T>?
  local metadata = runtimeMetadata(value)
  if metadata == nil or metadata.__element == nil then
    return nil
  end

  local sequence = value :: RuntimeTable
  if kind == "array" and sequence.array ~= true then
    return nil
  end

  if kind == "list" and metadata.__kind ~= "list" then
    return nil
  end

  if metadata.__element == element then
    return value :: Sequence<T>
  end

  local assignable = metadata.__assignable
  if covariant and (sequence.array == true or kind == "interface") and typeof(assignable) == "table" and (assignable :: RuntimeTable)[element] == true then
    return value :: Sequence<T>
  end

  return nil
end

function Collections.requireCast<T>(value: T?, original: unknown): T?
  if value == nil and original ~= nil then
    error("Invalid collection cast.")
  end

  return value
end

function Collections.range<T>(sequence: Sequence<T>, index: number, count: number): ()
  if index < 0 or (count < 0 or index > sequence.count - count) then
    error("Collection index or range is out of bounds.")
  end
end

function Collections.mutable<T>(sequence: Sequence<T>): ()
  if sequence.array or sequence.readonly then
    error("Collection has a fixed size.")
  end
end

function Collections.new<T>(array: boolean, length: number, default: T?, capacity: number, metadata: Descriptor, accept: (value: unknown) -> boolean): Sequence<T>
  if length < 0 or capacity < 0 then
    error("Collection size cannot be negative.")
  end

  local runtimeMetadata: Metadata = {
    __kind = if array then "array" else "list",
    __name = if array then metadata.__arrayName else metadata.__listName,
    __arrayName = metadata.__arrayName,
    __listName = metadata.__listName,
    __element = metadata.__element,
    __assignable = metadata.__assignable,
  }

  local sequence: Sequence<T> = {
    items = {},
    count = length,
    capacity = capacity,
    version = 0,
    array = array,
    readonly = false,
    accept = accept,
    default = default,
  }

  local i = 0
  while i < length do
    rawset(sequence.items, i + 1, default)
    i = i + 1
  end

  return setmetatable(sequence, setmetatable({}, runtimeMetadata))
end

function Collections.seal<T>(sequence: Sequence<T>, kind: Kind): ()
  sequence.array = kind == "array"
  sequence.readonly = kind == "readonly"

  local metadata = metadataOf(sequence)

  metadata.__kind = kind
  metadata.__name = metadata.__arrayName
end

function Collections.ArrayClear<T>(sequence: Sequence<T>, index: number, count: number): ()
  Collections.range(sequence, index, count)

  local endIndex = index + count
  while index < endIndex do
    rawset(sequence.items, index + 1, sequence.default)
    index = index + 1
  end
end

function Collections.get<T>(sequence: Sequence<T>, index: number): T
  Collections.range(sequence, index, 1)

  return sequence.items[index + 1]
end

function Collections.ToString<T>(sequence: Sequence<T>): string
  return metadataOf(sequence).__name
end

function Collections.set<T>(sequence: Sequence<T>, index: number, value: T): ()
  if sequence.readonly then
    error("Collection is read-only.")
  end

  Collections.range(sequence, index, 1)

  if sequence.accept(value) == false then
    error("Value is incompatible with the array element type.")
  end

  sequence.items[index + 1] = value
  sequence.version = sequence.version + 1
end

function Collections.EnsureCapacity<T>(sequence: Sequence<T>, capacity: number): number
  if capacity < 0 then
    error("Capacity cannot be negative.")
  end

  if capacity > sequence.capacity then
    sequence.capacity = math.max(capacity, if sequence.capacity == 0 then 4 else sequence.capacity * 2)
  end

  return sequence.capacity
end

function Collections.capacity<T>(sequence: Sequence<T>, capacity: number): ()
  if capacity < sequence.count then
    error("Capacity cannot be less than Count.")
  end

  sequence.capacity = capacity
end

function Collections.push<T>(sequence: Sequence<T>, value: T): ()
  Collections.EnsureCapacity(sequence, sequence.count + 1)
  sequence.items[sequence.count + 1] = value
  sequence.count = sequence.count + 1
  sequence.version = sequence.version + 1
end

function Collections.Add<T>(sequence: Sequence<T>, value: T): ()
  Collections.mutable(sequence)
  Collections.push(sequence, value)
end

function Collections.copy<T>(sequence: Sequence<T>, index: number, count: number, array: boolean): Sequence<T>
  Collections.range(sequence, index, count)

  local result = Collections.new(array, count, sequence.default, count, metadataOf(sequence), sequence.accept)
  local i = 0
  while i < count do
    result.items[i + 1] = sequence.items[index + i + 1]
    i = i + 1
  end

  return result
end

function Collections.ToArray<T>(sequence: Sequence<T>): Sequence<T>
  return Collections.copy(sequence, 0, sequence.count, true)
end

function Collections.Clone<T>(sequence: Sequence<T>): Sequence<T>
  return Collections.ToArray(sequence)
end

function Collections.GetRange<T>(sequence: Sequence<T>, index: number, count: number): Sequence<T>
  return Collections.copy(sequence, index, count, false)
end

function Collections.InsertRange<T>(sequence: Sequence<T>, index: number, source: Sequence<T>): ()
  Collections.mutable(sequence)
  Collections.range(sequence, index, 0)

  local copy = Collections.ToArray(source)
  local count = copy.count
  if count == 0 then
    return
  end

  Collections.EnsureCapacity(sequence, sequence.count + count)

  local i = sequence.count - 1
  while i >= index do
    sequence.items[i + count + 1] = sequence.items[i + 1]
    i = i - 1
  end

  i = 0
  while i < count do
    sequence.items[index + i + 1] = copy.items[i + 1]
    i = i + 1
  end

  sequence.count = sequence.count + count
  sequence.version = sequence.version + 1
end

function Collections.AddRange<T>(sequence: Sequence<T>, source: Sequence<T>): ()
  Collections.InsertRange(sequence, sequence.count, source)
end

function Collections.Insert<T>(sequence: Sequence<T>, index: number, value: T): ()
  Collections.mutable(sequence)
  Collections.range(sequence, index, 0)
  Collections.EnsureCapacity(sequence, sequence.count + 1)

  local i = sequence.count
  while i > index do
    sequence.items[i + 1] = sequence.items[i - 1 + 1]
    i = i - 1
  end

  sequence.items[index + 1] = value
  sequence.count = sequence.count + 1
  sequence.version = sequence.version + 1
end

function Collections.RemoveRange<T>(sequence: Sequence<T>, index: number, count: number): ()
  Collections.mutable(sequence)
  Collections.range(sequence, index, count)

  if count == 0 then
    return
  end

  local items = sequence.items
  local remaining = sequence.count - count
  local i = index
  while i < remaining do
    items[i + 1] = items[i + count + 1]
    i = i + 1
  end

  while i < sequence.count do
    rawset(items, i + 1, nil)
    i = i + 1
  end

  sequence.count = remaining
  sequence.version = sequence.version + 1
end

function Collections.RemoveAt<T>(sequence: Sequence<T>, index: number): ()
  Collections.RemoveRange(sequence, index, 1)
end

function Collections.Clear<T>(sequence: Sequence<T>): ()
  Collections.mutable(sequence)

  table.clear(sequence.items)
  sequence.count = 0
  sequence.version = sequence.version + 1
end

function Collections.equals(left: unknown, right: unknown): boolean
  if left == right then
    return true
  end

  if typeof(left) == "number" and left ~= left and right ~= right then
    return true
  end

  if typeof(left) ~= "table" or typeof(right) ~= "table" then
    return false
  end

  local leftObject = left :: RuntimeTable
  local rightObject = right :: RuntimeTable
  local equals = leftObject.Equals
  if typeof(equals) == "function" then
    return (equals :: (unknown, unknown) -> boolean)(left, right)
  end

  local leftMetadata = runtimeMetadata(left)
  local rightMetadata = runtimeMetadata(right)
  if leftMetadata == nil or rightMetadata == nil then
    return false
  end

  local boxedType = leftMetadata.__boxedType
  return boxedType ~= nil and boxedType == rightMetadata.__boxedType and Collections.equals(rawget(leftObject, "__value"), rawget(rightObject, "__value"))
end

function Collections.IndexOf<T>(sequence: Sequence<T>, value: T, index: number?, count: number?): number
  local first: number = index or 0
  local length: number = count or sequence.count - first

  Collections.range(sequence, first, length)

  local i = first
  while i < first + length do
    if Collections.equals(sequence.items[i + 1], value) then
      return i
    end

    i = i + 1
  end

  return -1
end

function Collections.LastIndexOf<T>(sequence: Sequence<T>, value: T, index: number?, count: number?): number
  local last: number = index or sequence.count - 1
  local length: number = count or last + 1

  if sequence.count == 0 then
    return -1
  end

  Collections.range(sequence, last - length + 1, length)

  if last >= sequence.count then
    error("Collection index is out of bounds.")
  end

  local i = last
  while i > last - length do
    if Collections.equals(sequence.items[i + 1], value) then
      return i
    end

    i = i - 1
  end

  return -1
end

function Collections.Contains<T>(sequence: Sequence<T>, value: T): boolean
  return Collections.IndexOf(sequence, value) >= 0
end

function Collections.Remove<T>(sequence: Sequence<T>, value: T): boolean
  Collections.mutable(sequence)

  local index = Collections.IndexOf(sequence, value)
  if index < 0 then
    return false
  end

  Collections.RemoveAt(sequence, index)

  return true
end

function Collections.Reverse<T>(sequence: Sequence<T>, index: number?, count: number?): ()
  local first: number = index or 0
  local length: number = count or sequence.count

  Collections.range(sequence, first, length)

  local last = first + length - 1
  while first < last do
    local value = sequence.items[first + 1]
    sequence.items[first + 1] = sequence.items[last + 1]
    sequence.items[last + 1] = value
    first = first + 1
    last = last - 1
  end

  sequence.version = sequence.version + 1
end

function Collections.TrimExcess<T>(sequence: Sequence<T>): ()
  if sequence.count < math.floor(sequence.capacity * 0.9) then
    sequence.capacity = sequence.count
  end
end

function Collections.CopyTo<T>(sequence: Sequence<T>, sourceIndex: number, destination: Sequence<T>, index: number, count: number): ()
  Collections.range(sequence, sourceIndex, count)
  Collections.range(destination, index, count)

  local copy = Collections.copy(sequence, sourceIndex, count, true)
  local i = 0
  while i < count do
    Collections.set(destination, index + i, copy.items[i + 1])
    i = i + 1
  end
end

function Collections.GetLength<T>(sequence: Sequence<T>, dimension: number): number
  if dimension ~= 0 then
    error("Array dimension is out of bounds.")
  end

  return sequence.count
end

function Collections.GetLowerBound<T>(sequence: Sequence<T>, dimension: number): number
  if dimension ~= 0 then
    error("Array dimension is out of bounds.")
  end

  return 0
end

function Collections.GetUpperBound<T>(sequence: Sequence<T>, dimension: number): number
  if dimension ~= 0 then
    error("Array dimension is out of bounds.")
  end

  return sequence.count - 1
end

local emptyArrays: { [string]: unknown } = {}

function Collections.empty<T>(key: string, factory: () -> T): T
  local value = emptyArrays[key]
  if value == nil then
    value = factory()
    emptyArrays[key] = value
  end

  -- Runtime type keys keep cached element types separate.
  return value :: T
end

function Collections.FindIndex<T>(sequence: Sequence<T>, index: number, count: number, predicate: ((T) -> boolean)?): number
  Collections.range(sequence, index, count)

  if predicate == nil then
    error("Predicate cannot be null.")
  end

  local test = predicate :: (T) -> boolean
  local remaining = count
  while remaining > 0 do
    if test(sequence.items[index + 1]) then
      return index
    end

    index = index + 1
    remaining = remaining - 1
  end

  return -1
end

function Collections.FindLastIndex<T>(sequence: Sequence<T>, index: number, count: number, predicate: ((T) -> boolean)?): number
  Collections.range(sequence, index - count + 1, count)

  if predicate == nil then
    error("Predicate cannot be null.")
  end

  local test = predicate :: (T) -> boolean
  local remaining = count
  while remaining > 0 do
    if test(sequence.items[index + 1]) then
      return index
    end

    index = index - 1
    remaining = remaining - 1
  end

  return -1
end

function Collections.Exists<T>(sequence: Sequence<T>, predicate: ((T) -> boolean)?): boolean
  return Collections.FindIndex(sequence, 0, sequence.count, predicate) >= 0
end

function Collections.Find<T>(sequence: Sequence<T>, predicate: ((T) -> boolean)?): T?
  local index = Collections.FindIndex(sequence, 0, sequence.count, predicate)

  return if index < 0 then sequence.default else sequence.items[index + 1]
end

function Collections.FindLast<T>(sequence: Sequence<T>, predicate: ((T) -> boolean)?): T?
  local index = Collections.FindLastIndex(sequence, sequence.count - 1, sequence.count, predicate)

  return if index < 0 then sequence.default else sequence.items[index + 1]
end

function Collections.TrueForAll<T>(sequence: Sequence<T>, predicate: ((T) -> boolean)?): boolean
  if predicate == nil then
    error("Predicate cannot be null.")
  end

  local test = predicate :: (T) -> boolean
  local i = 0
  while i < sequence.count do
    if test(sequence.items[i + 1]) == false then
      return false
    end

    i = i + 1
  end

  return true
end

function Collections.FindAll<T>(sequence: Sequence<T>, predicate: ((T) -> boolean)?): Sequence<T>
  if predicate == nil then
    error("Predicate cannot be null.")
  end

  local test = predicate :: (T) -> boolean
  local result = Collections.new(false, 0, sequence.default, 0, metadataOf(sequence), sequence.accept)
  local i = 0
  while i < sequence.count do
    if test(sequence.items[i + 1]) then
      Collections.push(result, sequence.items[i + 1])
    end

    i = i + 1
  end

  return result
end

function Collections.ForEach<T>(sequence: Sequence<T>, action: ((T) -> ())?): ()
  if action == nil then
    error("Action cannot be null.")
  end

  local run = action :: (T) -> ()
  local version = sequence.version
  local i = 0
  while i < sequence.count do
    if version ~= sequence.version then
      error("Collection was modified during enumeration.")
    end

    run(sequence.items[i + 1])
    i = i + 1
  end

  if version ~= sequence.version then
    error("Collection was modified during enumeration.")
  end
end

function Collections.ConvertAll<T, U>(sequence: Sequence<T>, converter: ((T) -> U)?, result: Sequence<U>): Sequence<U>
  if converter == nil then
    error("Converter cannot be null.")
  end

  local convert = converter :: (T) -> U
  local i = 0
  while i < sequence.count do
    Collections.push(result, convert(sequence.items[i + 1]))
    i = i + 1
  end

  return result
end

function Collections.RemoveAll<T>(sequence: Sequence<T>, predicate: ((T) -> boolean)?): number
  Collections.mutable(sequence)

  if predicate == nil then
    error("Predicate cannot be null.")
  end

  local test = predicate :: (T) -> boolean
  local items = sequence.items
  local read = 0
  local write = 0
  while read < sequence.count do
    if test(items[read + 1]) == false then
      items[write + 1] = items[read + 1]
      write = write + 1
    end

    read = read + 1
  end

  local removed = sequence.count - write
  if removed == 0 then
    return 0
  end

  read = write
  while read < sequence.count do
    rawset(items, read + 1, nil)
    read = read + 1
  end

  sequence.count = write
  sequence.version = sequence.version + 1

  return removed
end

function Collections.compare(left: unknown, right: unknown): number
  if typeof(left) == "number" and left ~= left then
    return if right ~= right then 0 else -1
  end

  if typeof(right) == "number" and right ~= right then
    return 1
  end

  if left == right then
    return 0
  end

  if left == nil then
    return -1
  end

  if right == nil then
    return 1
  end

  if typeof(left) == "boolean" and typeof(right) == "boolean" then
    return if left == false then -1 else 1
  end

  if typeof(left) == "number" and typeof(right) == "number" then
    return if left < right then -1 else 1
  end

  if typeof(left) == "string" and typeof(right) == "string" then
    return if left < right then -1 else 1
  end

  error("Values do not support default ordering.")
end

function Collections.Sort<T>(sequence: Sequence<T>, index: number, count: number, comparison: ((T, T) -> number)?): ()
  Collections.range(sequence, index, count)

  local compareWith: (T, T) -> number = comparison or Collections.compare

  local entries: { { value: T } } = {}
  local i = 0
  while i < count do
    entries[i + 1] = {
      value = sequence.items[index + i + 1],
    }
    i = i + 1
  end

  table.sort(entries, function(left: { value: T }, right: { value: T }): boolean
    return compareWith(left.value, right.value) < 0
  end)

  i = 0
  while i < count do
    sequence.items[index + i + 1] = entries[i + 1].value
    i = i + 1
  end

  sequence.version = sequence.version + 1
end

return Collections