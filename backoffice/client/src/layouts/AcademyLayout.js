import React from "react";

import AdminLayout from "layouts/AdminLayout.js";

import { academyRoutes } from "routes.js";

export default function Dashboard(props) {
  return (
    <AdminLayout
      {...props}
      routes={academyRoutes}
      layoutUrl={"/academy"}
      bgColor={"academy"}
      color={"blue"}
      navbarColor={"academy"}
      defaultUrl={"/academy/dashboard"}
      unit={1}
      moduleId={8}
      moduleName={"ECCSA"}
      moduleName2={"ECCSA Academy"}
    ></AdminLayout>
  );
}
