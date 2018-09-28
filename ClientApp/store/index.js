import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

// STATE
const state = {
  counter: 1,
  courses: null,
  units: null,
  topics: null,
  tipicsTotal: 0,
  unitsTotal: 0,
  coursesTotal: 0
}

// MUTATIONS
const mutations = {
  async fetchCourses (state, obj) {
    try {
      var from = obj.from
      var to = obj.to
      let response = await axios.get(`/api/courses?from=${from}&to=${to}`)
      state.courses = response.data.courses
      state.coursesTotal = response.data.total
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
  async removeCourse (state, id) {
    try {
      let response = await axios.delete(`/api/courses/${id}`)
      if (response.data) {
        const course = state.courses.filter(x => x.id === id)
        const index = state.courses.indexOf(course[0])
        state.courses.splice(index, 1)
      }
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
  async addCourse (state, obj) {
    try {
      let response = await axios.post(`/api/courses/`, obj)
      if (response.data) {
        state.courses.push(response.data)
      }
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
  async fetchUnits (state, obj) {
    try {
      var from = obj.from
      var to = obj.to
      let response = await axios.get(`/api/units?from=${from}&to=${to}`)
      state.units = response.data.units
      state.unitsTotal = response.data.total
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
  async removeUnit (state, id) {
    try {
      let response = await axios.delete(`/api/units/${id}`)
      if (response.data) {
        const unit = state.units.filter(x => x.id === id)
        const index = state.units.indexOf(unit[0])
        state.units.splice(index, 1)
      }
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
  async addUnit (state, obj) {
    try {
      let response = await axios.post(`/api/units/`, obj)
      if (response.data) {
        state.units.push(response.data)
      }
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
  async fetchTopics (state, obj) {
    try {
      var from = obj.from
      var to = obj.to
      let response = await axios.get(`/api/topics?from=${from}&to=${to}`)
      state.topics = response.data.topics
      state.topicsTotal = response.data.total
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
  async removeTopic (state, id) {
    try {
      let response = await axios.delete(`/api/topics/${id}`)
      if (response.data) {
        const topic = state.topics.filter(x => x.id === id)
        const index = state.topics.indexOf(topic[0])
        state.topics.splice(index, 1)
      }
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
  async addTopic (state, obj) {
    try {
      let response = await axios.post(`/api/topics/`, obj)
      if (response.data) {
        state.topics.push(response.data)
      }
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  }
}

// ACTIONS
const actions = ({
  fetchCourses ({ commit }, obj) {
    commit('fetchCourses', obj)
  },
  removeCourse ({ commit }, id) {
    commit('removeCourse', id)
  },
  addCourse ({ commit }, obj) {
    commit('addCourse', obj)
  },
  fetchUnits ({ commit }, obj) {
    commit('fetchUnits', obj)
  },
  removeUnit ({ commit }, id) {
    commit('removeUnit', id)
  },
  addUnit ({ commit }, obj) {
    commit('addUnit', obj)
  },
  fetchTopics ({ commit }, obj) {
    commit('fetchTopics', obj)
  },
  removeTopic ({ commit }, id) {
    commit('removeTopic', id)
  },
  addTopic ({ commit }, obj) {
    commit('addTopic', obj)
  }
})

export default new Vuex.Store({
  state,
  mutations,
  actions
})
