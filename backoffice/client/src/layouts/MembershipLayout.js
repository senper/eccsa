import React from "react";

import AdminLayout from "layouts/AdminLayout.js";

import membershipRoutes from "routes.js";

export default function Dashboard(props) {
  return (
    <AdminLayout
      {...props}
      routes={membershipRoutes}
      layoutUrl={"/membership"}
      bgColor={"membership"}
      color={"blue"}
      navbarColor={"membership"}
      unit={7}
      moduleId={1}
      defaultUrl={"/membership/dashboard"}
      moduleName={"ECCSA"}
      moduleName2={"ECCSA Membership"}
    ></AdminLayout>
  );
}
