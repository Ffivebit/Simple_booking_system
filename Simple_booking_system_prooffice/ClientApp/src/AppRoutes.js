import { TodayBookings } from "./components/TodayBookings";
import { AllBookings } from "./components/AllBookings";
import {Home} from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/today-booking',
    element: <TodayBookings />
  },
  {
    path: '/all-booking',
    element: <AllBookings />
  }
];

export default AppRoutes;
