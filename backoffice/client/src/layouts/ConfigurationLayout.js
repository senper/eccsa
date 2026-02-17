import React from "react";

import AdminLayout from "layouts/AdminLayout.js";

import { configurationRoutes } from "routes.js";

export default function Dashboard(props) {
  return (
    <AdminLayout
      {...props}
      routes={configurationRoutes}
      layoutUrl={"/configuration"}
      bgColor={"configuration"}
      navbarColor={"configuration"}
      color={"blue"}
      unit={5}
      moduleId={128}
      defaultUrl={"/configuration/dashboard"}
      moduleName={"ECCSA"}
      moduleName2={"ECCSA Configuration"}
    ></AdminLayout>
  );
}
