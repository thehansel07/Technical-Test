using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Core.Interface;

namespace WebApplication.Core.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        static HttpClient httpClient = new HttpClient();
        public async Task<T> AddAsync(T entity)
        {
            try
            {
                StringContent data = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://fakerestapi.azurewebsites.net/api/v1/Books", data);
                response.EnsureSuccessStatusCode();
                return entity;
            }
            catch (Exception ex)
            {

                throw new Exception("Error al Guardar Datos:" + ex);
            }

        }

        public async Task<T> DeleteAsync(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync("https://fakerestapi.azurewebsites.net/api/v1/Books/" + id);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return null;

                }

            }
            catch (Exception ex)
            {

                throw new Exception("Ha Ocurrido un error", ex);
            }
           

            return null;

        }

        public Task<T> FilterAsync(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {

            try
            {
                List<T> entityList = new List<T>();
                var response = await httpClient.GetAsync("https://fakerestapi.azurewebsites.net/api/v1/Books");
                response.EnsureSuccessStatusCode();
                string jsonArray = await response.Content.ReadAsStringAsync();
                if (jsonArray == null)
                {
                    return entityList;

                }
                return entityList = JsonConvert.DeserializeObject<List<T>>(jsonArray);
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error", ex);
            }


        }

        public async Task<IEnumerable<T>> GetByIdAsync(int id)
        {
            try
            {
                List<T> list = new List<T>();
                var response = await httpClient.GetAsync("https://fakerestapi.azurewebsites.net/api/v1/Books/" + id);
                string apiReponse = await response.Content.ReadAsStringAsync();
                if (apiReponse == "")
                {
                    return null;
                }
                var results = JsonConvert.DeserializeObject<dynamic>(apiReponse);
                JArray array = new JArray();
                array.Add(results);
                string datas = Convert.ToString(array);
                datas = datas.Trim().TrimStart('{').TrimEnd('}');
                return list = JsonConvert.DeserializeObject<List<T>>(datas);
            }

            catch (Exception e)
            {
                throw new Exception("Eror al Filtrar" + e);
            }
        }

        public async Task<T> Updatesync(T entity)
        {
            try
            {
                StringContent data = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync("https://fakerestapi.azurewebsites.net/api/v1/Books", data);
                response.EnsureSuccessStatusCode();
                return entity;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error", ex);
            }
        }
    }
}
