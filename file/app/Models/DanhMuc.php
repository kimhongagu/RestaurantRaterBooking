<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Database\Eloquent\Model;

class DanhMuc extends Model
{
    protected $table = 'danhmuc'; 

    protected $fillable = [
        'tendanhmuc', 
        'tendanhmuc_slug', 
        'loai', 
    ];

    public function sach()
    {
        return $this->belongsTo(Sach::class, 'sach_id', 'id');
    }

    public function vanPhongPham()
    {
        return $this->belongsTo(VanPhongPham::class, 'vpp_id', 'id');
    }

}
