import Courses from 'components/courses/index'
import Units from 'components/units/index'
import Topics from 'components/topics/index'
import HomePage from 'components/home-page'

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home' },
  { name: 'courses', path: '/courses', component: Courses, display: 'Courses' },
  { name: 'units', path: '/units', component: Units, display: 'Units' },
  { name: 'topics', path: '/topics', component: Topics, display: 'Topics' }
]
