<template>
  <div>
    <h1>Units</h1>

    <div v-if="!units" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>
    <div>
      Select Course:
      <select v-model="selectedCourse">
          <option v-for="(course, index) in courses"
                  :value="course.id">{{ course.id }}</option>
      </select>
    </div>
    <template v-if="units">
      <table class="table">
        <thead class="bg-dark text-white">
          <tr>
            <th>Id</th>
            <th>Sequence</th>
            <th>Description</th>
            <th>Course Id</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr :class="index % 2 == 0 ? 'bg-white' : 'bg-light'" v-for="(unit, index) in units" :key="index">
            <td>{{ unit.id }}</td>
            <td>{{ unit.sequence }}</td>
            <td>{{ unit.description }}</td>
            <td>{{ unit.courseId }}</td>
            <td><a class="btn btn-default" @click="removeUnit(unit.id)">delete</a></td>
          </tr>
        </tbody>
      </table>
      <nav aria-label="...">
        <a class="btn btn-default justify-content-md-end justify-content-sm-center" @click="addRandomUnit()">Add new</a>
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
      currentPage: 1,
      selectedCourse: null
    }
  },
  computed: {
    ...mapState({
      total: state => state.unitsTotal,
      units: state => state.units,
      courses: state => state.courses
    }),
    totalPages: function () {
      return Math.ceil(this.total / this.pageSize)
    }
  },
  methods: {
    ...mapActions(['fetchUnits', 'removeUnit', 'addUnit']),

    async loadPage (page) {
      this.currentPage = page
      var from = (page - 1) * (this.pageSize)
      var to = from + this.pageSize
      this.fetchUnits({
        from,
        to
      })
    },

    async addRandomUnit () {
      this.addUnit({
        description: "wubba labba dub dub",
        sequence: 1,
        courseId: this.selectedCourse
      })
    }
  },
  async mounted() {
    if (!this.units) {
      this.loadPage(1)
    }
  }
}
</script>
