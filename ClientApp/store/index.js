import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

// STATE
const state = {
  courses: null,
  units: null,
  topics: null
}

function deleteCourse (state, course) {
  // delete related units
  course.units.forEach(u => {
    let unit = state.units.filter(x => x.id == u.id)
    if (unit.length) {
      deleteUnit(state, unit[0])
    }
  })

  // delete course from state
  const index = state.courses.indexOf(course)

  if (index > -1) {
    state.courses.splice(index, 1)
  }
}

function deleteUnit (state, unit) {
  unit.topics.forEach(t => {
    let topic = state.topics.filter(x => x.id == t.id)
    if (topic.length) {
      deleteTopic(state, topic[0])
    }
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
  async fetchCourses (state) {
    try {
      let response = await axios.get(`/api/courses`)
      state.courses = response.data
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
      let response = await axios.get(`/api/units`)
      state.units = response.data
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
  async addUnit (state, obj) {
    try {
      let response = await axios.post(`/api/units/`, obj)
      if (response.data) {
        state.units.push(response.data)
        const course = state.courses.filter(x => x.id === response.data.courseId)
        if (course.length) {
          course[0].units.push(response.data)
        }
      }
    } catch (err) {
      window.alert(err)
      console.error(err)
    }
  },
  async fetchTopics (state) {
    try {
      let response = await axios.get(`/api/topics`)
      state.topics = response.data
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
        const unit = state.units.filter(x => x.id === response.data.unitId)
        if (unit.length) {
          unit[0].topics.push(response.data)
        }
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

  }
})

export default new Vuex.Store({
  state,
  mutations,
  actions
})
