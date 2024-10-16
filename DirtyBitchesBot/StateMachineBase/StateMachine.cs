using Infrastructure.Services.Impls;

namespace DirtyBitchesBot.StateMachineBase
{
    public static class StateMachine
    {
        public static async Task<State?> GetStateAsync(string key)
        {
            return await CacheService.Instance.GetValueAsync<State>(key);
        }

        public static async Task<State> SetStateAsync(string key, State value)
        {
            return await CacheService.Instance.SetValueAsync(key, value);
        }

        public static async Task RemoveStateAsync(string key)
        {
            await CacheService.Instance.RemoveDataAsync(key);
        }
        public static async Task<bool> IsDataCached(string key)
        {
            return await CacheService.Instance.IsDataCachedAsync(key);
        }
    }
}
