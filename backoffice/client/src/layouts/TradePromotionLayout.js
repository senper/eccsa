import React from "react";

import AdminLayout from "layouts/AdminLayout.js";

import { tradePromotionRoutes } from "routes.js";

export default function Dashboard(props) {
  return (
    <AdminLayout
      {...props}
      routes={tradePromotionRoutes}
      layoutUrl={"/tradePromotion"}
      defaultUrl={"/tradePromotion/dashboard"}
      bgColor={"tradePromotion"}
      color={"orange"}
      navbarColor={"tradePromotion"}
      unit={4}
      moduleId={64}
      moduleName={"ECCSA"}
      moduleName2={"ECCSA Trade Promotion"}
    ></AdminLayout>
  );
}
