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
  coursesAll: null,
  unitsAll: null,
  topicsTotal: 0,
  unitsTotal: 0,
  coursesTotal: 0,
  currentPageCourses: 1,
  currentPageUnits: 1,
  currentPageTopics: 1
}

function deleteCourse(state, course) {
  // delete related units
  const units = state.units.filter(x => x.courseId === course.id)
  units.forEach(u => {
      deleteUnit(state, u)
  })

  // delete course from state
  const index = state.courses.indexOf(course)

  if (index > -1) {
    state.courses.splice(index, 1)
  }
}
function deleteUnit(state, unit) {
  const topics = state.topics.filter(x => x.unitId === unit.id)
  topics.forEach(t => {
    deleteTopic(state, t)
  })

  const index = state.units.indexOf(unit)

  if (index > -1) {
    state.units.splice(index, 1)
  }
}
function deleteTopic (state, topic) {
  const index = state.topics.indexOf(topic)
  state.topics.splice(index, 1)
}

// MUTATIONS
const mutations = {
  async getAllCourses (state) {
    try {
      var from = 0
      var to = 1000000
      let response = await axios.get(`/api/courses?from=${from}&to=${to}`)
      state.coursesAll = response.data.courses
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
  async getAllUnits () {
    try {
      var from = 0
      var to = 1000000
      let response = await axios.get(`/api/units?from=${from}&to=${to}`)
      state.unitsAll = response.data.units
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
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
        if (course.length) {
          deleteCourse(state, course[0])
        }
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
  async editCourse (state, obj) {
    try {
      let response = await axios.put('/api/courses', obj)
      if (response.data) {
        const course = state.courses.filter(x => x.id === obj.id);
        if (course.length) {
          course[0].description = obj.description
        }
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
        if (unit.length) {
          deleteUnit(state, unit[0])
        }
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
  async editUnit (state, obj) {
    try {
      let response = await axios.put('/api/units', obj)
      if (response.data) {
        const unit = state.units.filter(x => x.id === obj.id);
        if (unit.length) {
          unit[0].description = obj.description
          unit[0].sequence = obj.sequence
        }
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
        if (topic.length) {
          deleteTopic(state, topic[0])
        }
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
  },
  async editTopic (state, obj) {
    try {
      let response = await axios.put('/api/topics', obj)
      if (response.data) {
        const topic = state.topics.filter(x => x.id === obj.id);
        if (topic.length) {
          topic[0].description = obj.description
        }
      }
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
  setCurrentPage(state, obj) {
    state[obj.prop] = obj.page
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
  editCourse ({ commit }, obj) {
    commit('editCourse', obj)
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
  editUnit ({ commit }, obj) {
    commit('editUnit', obj)
  },
  fetchTopics ({ commit }, obj) {
    commit('fetchTopics', obj)
  },
  removeTopic ({ commit }, id) {
    commit('removeTopic', id)
  },
  addTopic ({ commit }, obj) {
    commit('addTopic', obj)
  },
  editTopic ({ commit }, obj) {
    commit('editTopic', obj)
  },
  getAllCourses ({ commit }) {
    commit('getAllCourses')
  },
  getAllUnits ({ commit }) {
    commit('getAllUnits')
  },
  setCurrentPage ({ commit }, obj) {
    commit('setCurrentPage', obj)
  }
})

export default new Vuex.Store({
  state,
  mutations,
  actions
})
