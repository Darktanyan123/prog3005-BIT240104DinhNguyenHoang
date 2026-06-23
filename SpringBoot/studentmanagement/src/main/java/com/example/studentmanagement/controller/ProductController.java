package com.example.studentmanagement.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/product")
public class ProductController {

    @GetMapping("/detail/{id}")
    @ResponseBody
    public String detail(@PathVariable Integer id) {

        if (id <= 0) {
            return "Error: Product ID must be greater than 0";
        }

        return "Product ID = " + id;
    }

    @GetMapping("/category")
    @ResponseBody
    public String category(
            @RequestParam(required = false) String name) {

        if (name == null || name.trim().isEmpty()) {
            return "Error: Category name is required";
        }

        return "Category = " + name;
    }
}