import Courses from 'components/courses'
import Units from 'components/units'
import Topics from 'components/topics'
import HomePage from 'components/home-page'

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home' },
  { name: 'courses', path: '/courses', component: Courses, display: 'Courses' },
  { name: 'units', path: '/units', component: Units, display: 'Units' },
  { name: 'topics', path: '/topics', component: Topics, display: 'Topics' }
]
