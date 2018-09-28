<template>
  <div>
    <h1>Courses</h1>

    <div v-if="!courses" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <template v-if="courses">
      <table class="table">
        <thead class="bg-dark text-white">
          <tr>
            <th>Id</th>
            <th>Description</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr :class="index % 2 == 0 ? 'bg-white' : 'bg-light'" v-for="(course, index) in courses" :key="index">
            <td>{{ course.id }}</td>
            <td>{{ course.description }}</td>
            <td><a class="btn btn-default" @click="removeCourse(course.id)">delete</a></td>
          </tr>
        </tbody>
      </table>
      <nav aria-label="...">
        <a class="btn btn-default justify-content-md-end justify-content-sm-center" @click="addRandomCourse()">Add new</a>
        <ul class="pagination justify-content-md-end justify-content-sm-center">
          <li :class="'page-item' + (currentPage == 1 ? ' disabled' : '')">
            <a class="page-link" href="#" tabindex="-1" @click="loadPage(currentPage - 1)">Previous</a>
          </li>
          <li :class="'page-item' + (n == currentPage ? ' active' : '')" v-for="(n, index) in totalPages" :key="index">
            <a class="page-link" href="#" @click="loadPage(n)">{{n}}</a>
          </li>
          <li :class="'page-item' + (currentPage < totalPages ? '' : ' disabled')">
            <a class="page-link" href="#" @click="loadPage(currentPage + 1)">Next</a>
          </li>
        </ul>
      </nav>
    </template>
  </div>
</template>

<script>
 import { mapActions, mapState } from 'vuex'

 export default {
  data() {
    return {
      pageSize: 5,
      currentPage: 1
    }
  },
  computed: {
    ...mapState({
      total: state => state.coursesTotal,
      courses: state => state.courses
    }),
    totalPages: function () {
      return Math.ceil(this.total / this.pageSize)
    }
  },
  methods: {
    ...mapActions(['fetchCourses', 'removeCourse', 'addCourse']),

    async loadPage (page) {
      this.currentPage = page
      var from = (page - 1) * (this.pageSize)
      var to = from + this.pageSize
      this.fetchCourses({
        from,
        to
      })
    },

    async addRandomCourse () {
      this.addCourse({
        description: "wubba labba dub dub"
      })
    }
  },
  async mounted() {
    if (!this.courses) {
      this.loadPage(1)
    }
  }
}
</script>
