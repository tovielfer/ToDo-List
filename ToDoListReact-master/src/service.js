import axios from 'axios';

// const apiUrl = "http://localhost:5052"
axios.defaults.baseURL = "http://localhost:5052";

export default {
  getTasks: async () => {
    const result = await axios.get(`/items`).catch(err=>console.log(err))
    if(result.data!=[]) 
    return result.data;
  else
  return []
  },

  addTask: async(name)=>{
    const result = await axios.post(`/items`, name).catch(err=>console.log(err)) 
    return {};
  },

  setCompleted: async(id, isComplete)=>{
    let tasks=[];
    axios.get(`/items`).then(x=>{tasks=x.data
    let t=tasks.filter(x=>x.id==id)[0];
    t.isComplete=isComplete;
    axios.put(`/items/${t.id}`, t)}).catch(err=>console.log(err))
  },

  deleteTask:async(id)=>{
    await axios.delete(`/items/${id}`).catch(err=>console.log(err))
  }
};
