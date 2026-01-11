--!strict
--!optimize 2
--!native

local List = {}

local ListMetatable = {
	__index = function(self: any, key: any)
		if type(key) == "number" then
			return self._items[key + 1]
		end

		local method = List[key]
		if method then
			return method
		end

		if key == "Count" then
			return self._count
		end

		return nil
	end,

	__newindex = function(self: any, key: any, value: any)
		if type(key) == "number" then
			self._items[key + 1] = value
		else
			rawset(self, key, value)
		end
	end,

	__tostring = function(self)
		return `List<{self._count}>`
	end,
}

export type List<T> = {
	Count: number,

	Add: (self: List<T>, item: T) -> (),

	[number]: T,
}

type ListInternal<T> = {
	_items: { T },
	_count: number,
}

function List.new<T>(initialData: { T }?): List<T>
	local self = {
		_items = initialData or {} :: { T },
		_count = initialData and #initialData or 0,
	}

	return (setmetatable(self, ListMetatable) :: any) :: List<T>
end

function List.Add<T>(self: List<T>, item: T)
	local internal = (self :: any) :: ListInternal<T>

	internal._count += 1
	internal._items[internal._count] = item
end

function List.RemoveAt<T>(self: List<T>, index: number)
	local internal = (self :: any) :: ListInternal<T>

	local luaIndex = index + 1
	if luaIndex > internal._count or luaIndex < 1 then
		error(`List index access out of range. Was: {luaIndex}.`)
	end

	table.remove(internal._items, index)
	internal._count -= 1
end

return List
