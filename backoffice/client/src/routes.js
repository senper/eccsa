import MembershipDashboard from "views/membership/Dashboard/Dashboard.js";
import AcademyDashboard from "views/academy/Dashboard/Dashboard.js";
import TradePromotionDashboard from "views/trade_promotion/Dashboard/Dashboard.js";
import ConfigurationDashboard from "views/configuration/Dashboard/Dashboard.js";

// @material-ui/icons
import DashboardIcon from "@material-ui/icons/Dashboard";
import Associations from "views/membership/Associations/Associations";
import Applicants from "views/membership/Applicants/Applicants";
import AccountBalance from "@material-ui/icons/AccountBalance";
import HowToRegIcon from "@material-ui/icons/HowToReg";
import NaturePeopleIcon from "@material-ui/icons/NaturePeople";
import {
  Accessibility,
  Assessment,
  Bookmark,
  Person,
  Create,
  PersonAdd,
  Toc,
} from "@material-ui/icons";

import TradeFacilitation from "views/trade_promotion/TradeFacilitation/TradeFacilitation";
import TradeFacilitationDetail from "views/trade_promotion/TradeFacilitation/TradeFacilitationDetail";
import MessageTemplates from "views/configuration/MessageTemplates/MessageTemplates";
import MessageTemplateEdit from "views/configuration/MessageTemplates/MessageTemplateEdit";
import UserProfile from "views/UserProfile/UserProfile";
import RegisterPage from "views/UserProfile/RegisterPage.js";
import LoginPage from "views/UserProfile/LoginPage.js";
import AssociationDetail from "views/membership/Associations/AssociationDetail";
import Topics from "views/academy/Topic/Topics";
import TopicDetail from "views/academy/Topic/TopicDetail";

export const authRoutes = [
  {
    path: "/register-page",
    name: "Register",
    icon: Create,
    component: RegisterPage,
    layout: "/auth",
  },
  {
    path: "/login-page",
    name: "Login",
    icon: PersonAdd,
    component: LoginPage,
    layout: "/auth",
  },
];

var membershipRoutes = [
  {
    path: "/dashboard",
    name: "Dashboard",
    icon: DashboardIcon,
    component: MembershipDashboard,
    layout: "/membership",
  },
  {
    path: "/user",
    name: "User Profile",
    icon: Person,
    component: UserProfile,
    layout: "/membership",
    redirect: true,
  },
  {
    path: "/associations",
    name: "Associations",
    icon: AccountBalance,
    component: Associations,
    layout: "/membership",
  },
  {
    path: "/individuals",
    name: "Individuals",
    icon: NaturePeopleIcon,
    component: Associations,
    layout: "/membership",
  },
  {
    path: "/applicants",
    name: "Applicants",
    icon: HowToRegIcon,
    component: Applicants,
    layout: "/membership",
  },
  {
    path: "/issues",
    name: "Issues",
    icon: HowToRegIcon,
    component: Associations,
    layout: "/membership",
  },
  {
    path: "/meetings",
    name: "Meetings",
    icon: HowToRegIcon,
    component: Associations,
    layout: "/membership",
  },
  {
    path: "/association_detail",
    name: "Association Detail",
    component: AssociationDetail,
    layout: "/membership",
    redirect: true,
  },
  {
    path: "/message_templates",
    name: "Message Templates",
    icon: Bookmark,
    component: MessageTemplates,
    layout: "/membership",
    unit: 7,
  },
  {
    path: "/message_template_edit",
    name: "Message Template Edit",
    component: MessageTemplateEdit,
    redirect: true,
    layout: "/membership",
    unit: 7,
  },
];

export var academyRoutes = [
  {
    path: "/dashboard",
    name: "Dashboard",
    icon: DashboardIcon,
    component: AcademyDashboard,
    layout: "/academy",
  },
  {
    path: "/user",
    name: "User Profile",
    icon: Person,
    component: UserProfile,
    layout: "/academy",
    redirect: true,
  },
  {
    path: "/topics",
    name: "Topics",
    icon: Toc,
    component: Topics,
    layout: "/academy",
  },
  {
    path: "/topic_detail",
    name: "Topic Detail",
    icon: Toc,
    component: TopicDetail,
    layout: "/academy",
    redirect: true,
  },
  {
    path: "/message_templates",
    name: "Message Templates",
    icon: Bookmark,
    component: MessageTemplates,
    layout: "/academy",
    unit: 1,
  },
  {
    path: "/message_template_edit",
    name: "Message Template Edit",
    component: MessageTemplateEdit,
    //layout: "/configuration",
    redirect: true,
    layout: "/academy",
    unit: 1,
  },
];

export var tradePromotionRoutes = [
  {
    path: "/dashboard",
    name: "Dashboard",
    icon: DashboardIcon,
    component: TradePromotionDashboard,
    layout: "/tradePromotion",
  },
  {
    path: "/user",
    name: "User Profile",
    icon: Person,
    component: UserProfile,
    layout: "/tradePromotion",
    redirect: true,
  },
  {
    path: "/trade_facilitation",
    name: "Trade Facilitation",
    icon: Accessibility,
    component: TradeFacilitation,
    layout: "/tradePromotion",
  },
  {
    path: "/trade_facilitation_detail",
    name: "Trade Facilitation Detail",
    component: TradeFacilitationDetail,
    layout: "/tradePromotion",
    redirect: true,
  },
  {
    path: "/message_templates",
    name: "Message Templates",
    icon: Bookmark,
    component: MessageTemplates,
    layout: "/tradePromotion",
    unit: 4,
  },
  {
    path: "/message_template_edit",
    name: "Message Template Edit",
    component: MessageTemplateEdit,
    //layout: "/configuration",
    redirect: true,
    layout: "/tradePromotion",
    unit: 4,
  },
];

export var configurationRoutes = [
  {
    path: "/dashboard",
    name: "Dashboard",
    icon: DashboardIcon,
    component: ConfigurationDashboard,
    layout: "/configuration",
  },
  {
    path: "/user",
    name: "User Profile",
    icon: Person,
    component: UserProfile,
    layout: "/configuration",
    redirect: true,
  },
  {
    path: "/message_templates",
    name: "Message Templates",
    icon: Bookmark,
    component: MessageTemplates,
    layout: "/configuration",
    unit: 5,
  },
  {
    path: "/message_template_edit",
    name: "Message Template Edit",
    component: MessageTemplateEdit,
    layout: "/configuration",
    redirect: true,
    unit: 5,
  },
];

export default membershipRoutes;
