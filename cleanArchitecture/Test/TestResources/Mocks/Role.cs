namespace Test.TestResources.Mocks;

public static class Role
{
    public static CreateRoleDto CreateRole()
    {
        var path = @$"{GetRootPath()}TestResources\Jsons\Roles\addRoleRQ.json";
        return GetJson<CreateRoleDto>(path);
    }

    public static CreateRoleDto CreateRoleError()
    {
        var path = @$"{GetRootPath()}TestResources\Jsons\Roles\addRoleErrorRQ.json";
        return GetJson<CreateRoleDto>(path);
    }

    public static UpdateRoleDto UpdateRole()
    {
        var path = @$"{GetRootPath()}TestResources\Jsons\Roles\updateRoleRQ.json";
        return GetJson<UpdateRoleDto>(path);
    }

    public static UpdateRoleDto UpdateRoleError()
    {
        var path = @$"{GetRootPath()}TestResources\Jsons\Roles\updateRoleErrorRQ.json";
        return GetJson<UpdateRoleDto>(path);
    }

}
