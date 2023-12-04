using Assignment02.Dto;

namespace Assignment02.Services;

public class UserService
{
    private static List<UserDto> _userLists = new()
    {
        new()
        {
            Name = "Tom",
            Id = 1,
            Points = 0
        },
        new()
        {
            Name = "Tim",
            Id = 2,
            Points = 3500
        },
        new()
        {
            Name = "Katy",
            Id = 3,
            Points = 4900
        },
        new()
        {
            Name = "Li Hua",
            Id = 4,
            Points = 30000
        }
    };

    public async Task<UserDto> GetUserById(int userId)
    {
        // 模拟数据库查询，假设查询需要花费3s
        UserDto? user = _userLists
            .FirstOrDefault(u => u.Id == userId);
        await Task.Delay(3000);
        // 结束查询

        if (user is null)
        {
            throw new Exception($"找不到Id为{userId}的用户");
        }

        return user;
    }
}